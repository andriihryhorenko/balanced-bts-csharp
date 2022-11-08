# balanced-bts-csharp
balanced-bts-csharp

Sorting random datasets in 3 ways:
1. BTree building
2. Counting sort
3. Common .net OrderBy

And printing times of executing of each sorting:

![image](https://user-images.githubusercontent.com/112312750/200671323-f61fe5b3-4050-4c0f-9332-d64e3c71e1be.png)

L: number of a dataset

## How to run

```bash
cd ./balanced-bts/balanced-bts/

$ dotnet run -l=12 -n=12 -s=12
```

-n: number of random datasets
-s: delta between size of random databases
-l: size of the first dataset
