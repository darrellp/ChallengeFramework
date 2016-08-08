using System;
using System.Collections.Generic;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		/// <summary>
		/// We'll solve this by forming a "schedule" of events.  Events consist of cars arriving or the ferry landing on a side.
		/// The ferry only leaves in response to having just landed or in response to a car's arrival so ferry departures are
		/// subsumed under other events.  The car arrival events are placed into a queue and pulled off the end.  Before pulling
		/// a car event off the queue though, we peek and compare it with the next ferry event.  If the latter occurs first, then
		/// we use it as the next event rather than the car event.  Could have handled this with all the events in
		/// a single priority queue but since only the ferry arrival has to be checked against the car arrivals that would be a
		/// bit of overkill.  Other than that, we just determine the next event, update the simulation time and arrange for
		/// everything to be updated correctly.
		/// </summary>
		[Challenge("UVA", "Ferry Loading III",
			"https://uva.onlinejudge.org/index.php?option=com_onlinejudge&Itemid=8&category=21&page=show_problem&problem=1842")]
		// ReSharper disable once InconsistentNaming
		public class FerryLoadingIII : IChallenge
		{
			public void Solve()
			{
				var cCases = GetVal();
				bool fFirstCase = true;
				for (var iCase = 0; iCase < cCases; iCase++)
				{
					var ferryCase = new FerryCase();

					ferryCase.SolveCase(fFirstCase);
					fFirstCase = false;
				}
			}

		    class Car
		    {
			    public Car(int arrivalTime, bool arriveRight, int index)
			    {
				    ArrivalTime = arrivalTime;
				    ArriveRight = arriveRight;
				    Index = index;
			    }

			    public int ArrivalTime { get; private set; }
			    private bool ArriveRight { get; set; }
			    public int Index { get; private set; }

			    public bool ArriveLeft
			    {
				    get { return !ArriveRight; }
			    }
		    }

		    class FerryCase
		    {
			    private readonly int _capacity;
			    private readonly int _crossingTime;
			    private readonly int _cCars;
			    private readonly Queue<Car> _incomingCars;
			    private int _simulationTime;
			    private int _ferryArrival;

			    readonly List<Car> _carsLoaded = new List<Car>();
			    // Cars waiting at the right and left bank
			    readonly Queue<Car> _carsRight = new Queue<Car>();
			    readonly Queue<Car> _carsLeft = new Queue<Car>();
			    bool _ferryOnLeft = true;

			    // Our final result - time each car waited
			    readonly int[] _unloadTimes;
			    bool _fCarsInTransit = true;

			    public FerryCase()
			    {
				    var vals = GetVals();
				    _capacity = vals[0];
				    _crossingTime = vals[1];
				    _cCars = vals[2];
				    _incomingCars = new Queue<Car>(_cCars);
				    _unloadTimes = new int[_cCars];

				    for (var iCar = 0; iCar < _cCars; iCar++)
				    {
					    // ReSharper disable once PossibleNullReferenceException
					    var carVals = Console.ReadLine().Split(' ');
					    var arrival = int.Parse(carVals[0]);
					    var arriveRight = carVals[1] == "right";
					    _incomingCars.Enqueue(new Car(arrival, arriveRight, iCar));
				    }
			    }

			    public void SolveCase(bool fFirstCase)
			    {
				    _ferryArrival = -1;

				    while (_fCarsInTransit)
				    {
					    if (_ferryArrival > 0 && (_incomingCars.Count == 0 || _incomingCars.Peek().ArrivalTime > _ferryArrival))
					    {
						    // Next event is ferry arrival

						    var ferryHeadsBackImmediately = _carsRight.Count != 0 || _carsLeft.Count != 0;
						    _simulationTime = _ferryArrival;
						    foreach (var car in _carsLoaded)
						    {
							    _unloadTimes[car.Index] = _simulationTime;
						    }
						    _carsLoaded.Clear();
						    _fCarsInTransit = _incomingCars.Count != 0 || ferryHeadsBackImmediately;
						    _ferryOnLeft = !_ferryOnLeft;
						    if (!ferryHeadsBackImmediately)
						    {
							    _ferryArrival = -1;
						    }
						    else
						    {
							    _ferryArrival = _simulationTime + _crossingTime;
							    LoadFerry();
						    }
					    }
					    else
					    {
						    // Next event is a car arrival
						    var carArriving = _incomingCars.Dequeue();
						    var carsQueued = carArriving.ArriveLeft ? _carsLeft : _carsRight;
						    _simulationTime = carArriving.ArrivalTime;
						    carsQueued.Enqueue(carArriving);
						    if (_ferryArrival < 0)
						    {
							    _ferryArrival = _simulationTime + _crossingTime;
							    LoadFerry();
						    }
					    }
				    }
				    if (!fFirstCase)
				    {
					    Console.WriteLine();
				    }
				    for (int iCar = 0; iCar < _cCars; iCar++)
				    {
					    Console.WriteLine(_unloadTimes[iCar]);
				    }
			    }

			    private void LoadFerry()
			    {
				    var carsQueued = _ferryOnLeft ? _carsLeft : _carsRight;
				    var cCarsToLoad = Math.Min(carsQueued.Count, _capacity);

				    for (var iLoadCar = 0; iLoadCar < cCarsToLoad; iLoadCar++)
				    {
					    _carsLoaded.Add(carsQueued.Dequeue());
				    }
			    }
		    }

			public string RetrieveSampleInput()
			{
				return @"
2
2 10 10
0 left
10 left
20 left
30 left
40 left
50 left
60 left
70 left
80 left
90 left
2 10 3
10 right
25 left
40 left";
			}

			public string RetrieveSampleOutput()
			{
				return @"
10
30
30
50
50
70
70
90
90
110

30
40
60
";
			}
		}
	}
}
