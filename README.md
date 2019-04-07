# GPS.Collections

Library of data structures for creating spacially oriented data.

## GPS.Collections.LinkedArray&lt;T&gt;

### Installation

__GPS.Collections__ is available through Nuget.

```powershell

nuget install GPS.Collections
```



### Description of LinkedArray

The `LinkedArray` is an auto-growing linked list of arrays that provide a contiguous indexing mechanism.  Indices may be any `Int32` value, both positive and negative.  This allows for natural indexing of data without sliding offsets.

### LinkedArray Usage

##### Negative Indices

```csharp

var linkedArray = new LinkedArray<string>();

linkedArray[-1] = "negative one";
linkedArray[-2] = "negative two";

Console.WriteLine($"Range: {linkedArray.Lowest} to {linkedArray.Highest}");
// Range: -2 to -1

```

##### Sparse Indices

```csharp

var linkedArray = new LinkedArray<string>();

linkedArray[-1000] = "negative one-thousand";
linkedArray[1000] = "one-thousand";

Console.WriteLine($"Range: {linkedArray.Lowest} to {linkedArray.Highest}");
// Range: -1000 to 1000

Console.WriteLine($"Count: {linkedArray.Count}");
// Count: 2001

```

##### Auto-growing

```csharp

var linkedArray = new LinkedArray<string>();

linkedArray[-1] = "negative one";
linkedArray[1] = "one";

Console.WriteLine($"Count: {linkedArray.Count}");
// Count: 3

linkedArray[3] = "three";
Console.WriteLine($"Count: {linkedArray.Count}");
// Count: 5

linkedArray[2] = "two";
Console.WriteLine($"Count: {linkedArray.Count}");
// Count: 5

```

##### Add Range

```csharp

var linkedArray = new LinkedArray<string>();

linkedArray[0] = "zero";
linkedArray.AddRange(new [] { "one", "two" } );
Console.WriteLine($"Range: {linkedArray.Lowest} to {linkedArray.Highest}");
// Range: 0 to 2

```

##### Add Range At

```csharp

var linkedArray = new LinkedArray<string>();

linkedArray.AddRange(21, new [] { "twent-one", "twenty-two" } );
Console.WriteLine($"Range: {linkedArray.Lowest} to {linkedArray.Highest}");
// Range: 21 to 22

```

##### IndexOf

```csharp

var linkedArray = new LinkedArray<string>();

linkedArray[10] = "find me";
linkedArray.AddRange(new [] { "me too", "me three" } );
Console.WriteLine($"IndexOf: \"find me\" = {linkedArray.IndexOf("find me")}");
// IndexOf: "find me" = 10

Console.WriteLine($"IndexOf: \"me three\" = {linkedArray.IndexOf("me three")}");
// IndexOf: "me three" = 12

```
