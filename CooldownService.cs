using System;
using System.Collections.Concurrent;

namespace WowGameInfo
{
    public class CooldownService
    {
        private readonly ConcurrentDictionary<string, DateTime> _cooldowns = new();
        private readonly TimeSpan _defaultCooldown = TimeSpan.FromSeconds(10); // ⏳ Modifiable

        public bool IsOnCooldown(ulong userId, string commandName, out TimeSpan remaining)
        {
            string key = $"{userId}:{commandName}";
            if (_cooldowns.TryGetValue(key, out var lastUsed))
            {
                var elapsed = DateTime.UtcNow - lastUsed;
                if (elapsed < _defaultCooldown)
                {
                    remaining = _defaultCooldown - elapsed;
                    return true;
                }
            }

            remaining = TimeSpan.Zero;
            return false;
        }

        public void SetCooldown(ulong userId, string commandName)
        {
            string key = $"{userId}:{commandName}";
            _cooldowns[key] = DateTime.UtcNow;
        }
    }
}
