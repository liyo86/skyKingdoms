public interface IAttackCooldown
{
    bool IsDefenseCooldown { get; }
    float DefenseCooldown { get; }
    bool IsShootCooldown { get; }
    float ShootCooldown { get; }
    bool IsSpecialAttackCooldown { get; }
    float SpecialAttackCooldown { get; }
}