[HttpPost("complete-task")]
public async Task<IActionResult> CompleteTask([FromBody] TaskRequest request)
{
    var user = await _context.Users
        .FirstOrDefaultAsync(u => u.TelegramId == request.TelegramId);

    if (user == null)
        return NotFound("User not found");

    int lemonsToAdd = request.TaskType.ToLower() switch
    {
        "progress tree" => 100,
        "watch video" => 100,
        "wash dishes" => 300,
        "clean floors" => 400,
        "read book" => 10000,
        _ => 0
    };

    if (lemonsToAdd == 0)
        return BadRequest("Unknown task type");

    user.UpdateActivity();
    user.AddLemons(lemonsToAdd);
    user.CheckAchievements(request.TaskType);

    await _context.SaveChangesAsync();

    return Ok(MapToUserInfo(user));
}