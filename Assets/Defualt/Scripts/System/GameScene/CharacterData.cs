using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    public Dictionary<string, object> characterData { get; private set; } // ĳ���� ������ ����


    private void Awake()
    {
        GameManager.Instance.dataManager.characterData = this; // ������ �Ŵ����� ĳ���� ������ ����
    }

    public void SetCharacterData(Dictionary<string, object> characterData)
    {
        this.characterData = characterData; // ĳ���� ������ ����
        CalculateAndSetStats();
    }

    private void CalculateAndSetStats()
    {
        if (this.characterData == null) return;

        Dictionary<string, int> stats = ExtractStats();

        string jobStr = characterData["job"].ToString();
        string raceStr = characterData["tribe"].ToString();
        int level = Convert.ToInt32(characterData["level"]);

        ApplyStatGrowth(jobStr, raceStr, level, ref stats);

        CalculateDerivedStats(ref stats);

        UpdateCharacterData(stats);
    }

    private Dictionary<string, int> ExtractStats()
    {
        List<string> statKeys = new List<string>
        {
            "str", "dex", "int", "spi", "vit", "pie", "dh", "det", "crt", "sks", "sps", "ten", "pie", "def", "mdf", "luk"
        };

        Dictionary<string, int> stats = new Dictionary<string, int>();
        foreach (var key in statKeys)
        {
            stats[key] = Convert.ToInt32(characterData.ContainsKey(key) ? characterData[key] : 0);
        }

        return stats;
    }

    private void ApplyStatGrowth(string job, string race, int level, ref Dictionary<string, int> stats)
    {    
        // ��� ĳ���Ϳ� ���� ������ �� ��� �ɷ�ġ �⺻ ��� ����
        const int BASE_STAT_GROWTH = 1; // �������� ��� �ɷ�ġ�� 1�� �⺻������ ����
        foreach (var key in stats.Keys.ToList())
        {
            stats[key] += BASE_STAT_GROWTH * level;
        }

        // ������ �ɷ�ġ ��� ����
        switch (job)
        {
            case "Warrior":
                stats["str"] += level * 3;
                stats["vit"] += level * 2;
                break;
            case "Dragoon":
                stats["str"] += level * 3;
                stats["dex"] += level * 2;
                break;
            case "Bard":
                stats["dex"] += level * 3;
                stats["crt"] += level * 2;
                break;
            case "BlackMage":
                stats["int"] += level * 4;
                stats["spi"] += level;
                break;
            case "WhiteMage":
                stats["spi"] += level * 4;
                stats["int"] += level;
                break;
        }

        // ������ �ɷ�ġ ��� ����
        switch (race)
        {
            case "Human":
                // �ΰ��� ��� �ɷ�ġ�� �յ��ϰ� ���
                foreach (var key in stats.Keys.ToList())
                {
                    stats[key] += level;
                }
                break;
            case "Elf":
                // ������ ��ø���� ���ŷ¿� ����
                stats["dex"] += level * 2;
                stats["spi"] += level * 2;
                break;
            case "Dwarf":
                // ������� ü�°� ���¿� ����
                stats["vit"] += level * 3;
                stats["def"] += level * 2;
                break;
        }
    }


    private void CalculateDerivedStats(ref Dictionary<string, int> stats)
    {
        // ���⿡�� �Ļ��� ����(���� ���ݷ�, ���� ���ݷ� ��)�� ���
        float dhMultiplier = CalculateMultiplier(stats["dh"]);
        float detMultiplier = CalculateMultiplier(stats["det"]);

        // ������ ���� �� ���� ����
        string job = characterData["job"].ToString();
        int mainStat = job == "Warrior" ? stats["str"] : stats["dex"];

        // ���� ���ݷ�, ���� ���ݷ� �� ���
        int pap = (int)((mainStat * 1.5) + (stats["dh"] * dhMultiplier) + (stats["det"] * detMultiplier));
        int map = (int)((stats["int"] * 1.5) + (stats["dh"] * dhMultiplier) + (stats["det"] * detMultiplier));
        int mhp = (int)((stats["spi"] * 1.5) + (stats["dh"] * dhMultiplier) + (stats["det"] * detMultiplier));
        int mph = (int)((stats["pie"] * CalculateMultiplier(stats["pie"])) * 1.5);
        int maxHp = (int)(stats["vit"] * CalculateStatHealthMultiplier(stats["vit"]));

        // �ڿ� ȸ���� ���
        int hpRecovery = CalculateHpRecovery(stats["vit"], stats["det"]);

        // ���� ������ �ٽ� characterData�� ����
        characterData["pap"] = pap;
        characterData["map"] = map;
        characterData["mhp"] = mhp;
        characterData["mph"] = mph;
        characterData["maxHp"] = maxHp;
        characterData["hpRecovery"] = hpRecovery;
    }

    private float CalculateMultiplier(int statValue)
    {
        float baseValue = 1.5f;
        float currentThreshold = baseValue;
        float multiplier = 1.2f; // �ʱ� ���� ����
        float decrement = 0.05f; // ���ҷ� ����

        while (statValue >= currentThreshold)
        {
            multiplier -= decrement; // �� �������� ���ҷ� ����
            if (decrement > 0.01f) decrement -= 0.01f; // ���ҷ� ���������� ����
            currentThreshold *= baseValue; // ���� ������ �Ӱ谪 ����
        }

        return multiplier;
    }

    private float CalculateStatHealthMultiplier(int vit)
    {
        int bitCount = BitCount(vit);
        return 10f + (0.5f * bitCount); // �⺻ ����ġ�� ��Ʈ ���� ���� �߰� ����ġ ����
    }

    // ü�� �ڿ� ȸ�� ��� �޼���
    private int CalculateHpRecovery(int vit, int det)
    {
        // vit�� det�� �տ� ����� ȸ���� ���
        int recoveryAmount = vit + (int)(det * 0.5); // det�� ����ġ 0.5�� ����

        return recoveryAmount;
    }

    private int BitCount(int value)
    {
        int count = 0;
        while (value > 0)
        {
            count += value & 1;
            value >>= 1; // ���������� ��Ʈ ����Ʈ
        }
        return count;
    }

    private void UpdateCharacterData(Dictionary<string, int> stats)
    {
        foreach (var stat in stats)
        {
            characterData[stat.Key] = stat.Value;
        }
    }
}
