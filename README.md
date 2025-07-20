# RePool

unity object pool

## Installation

```
https://github.com/stupeak/RePool.git?path=Assets/RePool
```

### Pool Containers
```C#
// base interface for pool containers
interface IPoolContainer<T>

// UnityEngine.Component pool
class ComponentPool<T>

// ComponentPool<TValue> with keys
class MultiComponentPool<TKey, TValue>

// UnityEngine.GameObject pool
class GameObjectPool
```

## Examples

```C#
using Stupeak.RePool;

class Bullet { }

ComponentPool<Bullet> bulletPool = new(bulletPrefab, bulletParent);

Bullet bulletInstance = bulletPool.Rent();
// adding a new instance
bulletPool.AddExisting(new Bullet());
// return to pool
bulletPool.Return(bulletInstance);
// release instance 
bulletPool.Release(bulletInstance);
```

###
