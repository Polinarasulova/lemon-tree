public class User
{
    public int Id { get; set; }
    public long TelegramId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; }

    public int Lemons { get; set; } = 0;
    public int Level { get; set; } = 1;

    public DateTime LastActiveTime { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public DateTime? LastDailyBonusClaimed { get; set; }
    public TimeSpan TotalOnlineTime { get; set; } = TimeSpan.Zero;

    public List<string> Achievements { get; set; } = new();

    // Метод обновления уровня
    public void UpdateLevel()
    {
        var newLevel = 1;
        if (Lemons >= 1000) newLevel = 5;
        else if (Lemons >= 500) newLevel = 4;
        else if (Lemons >= 200) newLevel = 3;
        else if (Lemons >= 100) newLevel = 2;

        if (newLevel > Level)
        {
            Level = newLevel;
            CheckAndAddAchievement($"🏆 Достигнут уровень {Level}");
        }
    }

    // Метод проверки достижений
    public void CheckAchievements(string taskType)
    {
        switch (taskType.ToLower())
        {
            case "read book" when !Achievements.Contains("📚 Прочитал книгу"):
                Achievements.Add("📚 Прочитал книгу");
                break;
            case "clean floors" when !Achievements.Contains("🧽 Почистил полы"):
                Achievements.Add("🧽 Почистил полы");
                break;
        }

        if (Lemons >= 1000 && !Achievements.Contains("💰 Накоплено 1000+ лимонов"))
        {
            Achievements.Add("💰 Накоплено 1000+ лимонов");
        }

        if ((DateTime.UtcNow - LastLoginDate).TotalDays >= 3 &&
            !Achievements.Contains("🔥 Активен 3 дня подряд"))
        {
            Achievements.Add("🔥 Активен 3 дня подряд");
        }
    }

    public void CheckAndAddAchievement(string achievement)
    {
        if (!Achievements.Contains(achievement))
        {
            Achievements.Add(achievement);
        }
    }

    public void AddLemons(int amount)
    {
        Lemons += amount;
        UpdateLevel();
    }

    public void UpdateActivity()
    {
        var now = DateTime.UtcNow;

        if (LastActiveTime != default)
        {
            TotalOnlineTime += now - LastActiveTime;
        }

        if (LastLoginDate.HasValue && (now - LastLoginDate.Value).TotalDays >= 1)
        {
            CheckAndAddAchievement("📅 Посетил сайт не первый день");
        }

        LastActiveTime = now;
        LastLoginDate = now;
    }

    public bool CanClaimDailyBonus()
    {
        return LastDailyBonusClaimed == null || 
               (DateTime.UtcNow - LastDailyBonusClaimed.Value).TotalDays >= 1;
    }

    public void ClaimDailyBonus()
    {
        AddLemons(500); // Бонус за вход
        LastDailyBonusClaimed = DateTime.UtcNow;
    }
}