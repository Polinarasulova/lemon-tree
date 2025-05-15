[HttpPost("login")]
public async Task<IActionResult> TelegramLogin([FromBody] TelegramAuthRequest request)
{
    if (!TelegramAuthHelper.ValidateTelegramData(request))
    {
        return BadRequest("Invalid Telegram data");
    }

    var user = await _context.Users
        .FirstOrDefaultAsync(u => u.TelegramId == request.Id);

    if (user == null)
    {
        user = new User
        {
            TelegramId = request.Id,
            Username = request.Username,
            FirstName = request.FirstName,
            LastName = request.LastName,
            LastActiveTime = DateTime.UtcNow
        };
        await _context.Users.AddAsync(user);
    }

    user.UpdateActivity();

    if (user.CanClaimDailyBonus())
    {
        user.ClaimDailyBonus();
    }

    user.UpdateLevel(); // На случай, если уровень уже был
    user.CheckAchievements(""); // Проверка общих достижений

    await _context.SaveChangesAsync();

    return Ok(MapToUserInfo(user));
}