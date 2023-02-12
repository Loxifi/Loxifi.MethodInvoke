A shorthand method of calling a reflective invoke on an object. 

```
myObject.Invoke<TResult>("MyMethod", arg1, arg2, arg3);
```

Finds a method with the given name matching the provided parameters and then casts the result to the provided type

Additional information will be provided on full release