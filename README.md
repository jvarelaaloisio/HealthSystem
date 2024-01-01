# Health System
This is a simple system which contains a [Health](https://github.com/jvarelaaloisio/HealthSystem/blob/main/Projects/HealthSystem/Health.cs) class. It tracks it's Health-Points (HP), can be damaged and healed and has callbacks for any internal value changes.

### Overflow
[AllowOverflow](https://github.com/jvarelaaloisio/HealthSystem/blob/a9f5797a3e2f47ff9a1f497e68c63110d0fc8c9b/Projects/HealthSystem/Health.cs#L18C23-L18C23) is a flag that tells the Health class to either:
- FALSE: clamp it's HP between 0 and maxHP
- TRUE: only clamp it's HP to not be lower than 0, but allow it to go over it's max value.

### Callback events
- [OnDamage](https://github.com/jvarelaaloisio/HealthSystem/blob/a9f5797a3e2f47ff9a1f497e68c63110d0fc8c9b/Projects/HealthSystem/Health.cs#L29C47-L29C56) is raised every time TakeDamage() is called.
- [OnHeal](https://github.com/jvarelaaloisio/HealthSystem/blob/a9f5797a3e2f47ff9a1f497e68c63110d0fc8c9b/Projects/HealthSystem/Health.cs#L35C47-L35C54) is raised every time Heal() is called.
- [OnDeath](https://github.com/jvarelaaloisio/HealthSystem/blob/a9f5797a3e2f47ff9a1f497e68c63110d0fc8c9b/Projects/HealthSystem/Health.cs#L23C29-L23C37) is raised when HP goes down to 0.

### Testing
This system is fully unit tested so to avoid any bugs, you can rest asure that this system will be  bug free.

### Some clarifications
#### Death
Health class doesn't handle death, so after going down to 0 HP, it will still take damage, only it won't have any effect. A "dead" health can also be healed. This class asumes death handling will be done outside.

#### Forks and collaborations
You're of course free to Fork this project and can open a Pull-Request if you have nice changes to it. The only requirement for merge is for all the current tests to be passed and to add all nedded tests for any new features being added.