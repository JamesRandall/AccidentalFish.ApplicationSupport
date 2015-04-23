The in memory repository provides an implementation of the unit of work and repository pattern that
enforces the traditional transactional behaviour of such a pattern when applied against an ACID data store
but using memory only.

It is designed primarily for use in testing scenarios and it is likely that, for some time, numerous performance
issues will exist when used with large numbers of objects or a high degree of concurrency.