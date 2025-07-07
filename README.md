# Stupeak.Pooling


## Installation

```
https://github.com/stupeak/Pooling.git?path=Assets/Pooling
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
