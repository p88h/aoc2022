p88h's Advent of Code 2022
==========================

This is a repository containing solutions for the 2022 Advent of Code (https://adventofcode.com/).

This years language of choice is C#, and the proper solutions are all in the lib/ directory, one per day + some common dispatch code in main.cs.
I'm using dotnet 6.0, YMMV on anything else than that.

If you would like to run this yourself, you can do it like so, from the top-level directory of the project:

```
$ dotnet run
Day 00 part 1: 10
Day 00 part 2: 24
```

This runs the last viable day (one for which an input exists in the inputs/ directory) and outputs it to standard out.

The following additional options are supported:

* `dotnet run 01` (or any other day name, but need leading zeroes for first 9) will run that particular day
* `dotnet run 01 02 03,04,07` will select multiple days (1 through 4 and 7)
* `dotnet run all` will run all of the daily modules

Benchmarking
============

You can also run in benchmark mode:

```
$ dotnet run -c Release bench
Selected days: 00
Day 00 part 1: 24 ns, 40990622 it/s
Day 00 part 2: 23 ns, 41771279 it/s
```

You can append `all` or selected days to the runner same as in regular mode.
Benchmarks from my system are also included in [BENCHMARKS.md](BENCHMARKS.md)

Visualisations
==============

The vis/ directory contains visualisations code - also in C#, and utilizing raylib and/or some ASCII art (but also via raylib)

To run them, pass an additional `vis` parameter, like so:

```
$ dotnet run -c Release vis 01
```

Visualisations can also write video to `DayXX.mp4` files automatically, if you pass additional 'rec' parameter.

```
$ dotnet run -c Release vis 03 rec
```

This requires FFMPEG and a bit of luck (seems to work fine under Linux/WSL2, and not quite fine on OSX)

The simplest way to view them is probably on
[YouTube](https://www.youtube.com/playlist?list=PLgRrl8I0Q168jJYjfbzak3l-9xkLU6bCE)

Copyright disclaimer
====================

Licensed under the Apache License, Version 2.0 (the "License");
you may not use these files except in compliance with the License.
You may obtain a copy of the License at

   https://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

