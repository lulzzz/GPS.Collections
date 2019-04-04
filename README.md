# GPS.Collections

Library of data structures for creating spacially oriented data.

## Benchmark

[Benchmark Results](https://1drv.ms/x/s!At7D0T4Uz1vyiNpl3sgdCIADmRFAcA)

1 - Sum of 20 Runs
2 - Average of 20 Runs
|Operation | 1 - ms	| 2 - ms |
|-|----------------|--------------------|
|Loading ArrayList Total|51|2.55|
| Loading LinkedArray Total | 59 | 2.95 |
| Loading LinkedList Total | 43 | 2.15 |
| Loading List Total | 1 | 0.05 |
| Find Last ArrayList Total | 0 | 0 |
| Find Last LinkedArray Total | 1 | 0.05 |
| Find Last LinkedList Total | 1 | 0.05 |
| Find Last List Total | 0 | 0 |
| Clear LinkedArray Total | 0 | 0 |
| Clear LinkedList Total | 5 | 0.25 |
| Clear List Total | 0 | 0 |
| Positive Random Set LinkedArray Total | 98 | 4.9 |
| Positive Random Set List Total | 2469 | 123.45 |
| Open Ended Random Set LinkedArray Total | 106 | 5.3 |

__Notes:__

* ArrayList does not have a Clear() method.
* LinkedList and ArrayList do not support inserting data arbitrarily in position.
* List, LinkedList and ArrayList do not support inserting data with open ended indices (Int32.MinValue … Int32.MaxValue).

> _*Randomly adding data to the List required on average 24x longer to add new items.*_

See test code at [LinkedArray_Tests.cs](https://github.com/gatewayprogrammingschool/GPS.Collections/blob/34e25069abe5f2978f7be9fa45fe3fbfe8dd8b9f/src/GPS.Collections.Tests/LinkedArray_Tests.cs#L152-L223)

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