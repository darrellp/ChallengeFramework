module DAP.EulerProblems.Utility
open System
open DAP.EulerProblems.PrimeList

let truncateIf p (s:#seq<'a>) : seq<'a> =
    seq {
        let e = s.GetEnumerator()
        while e.MoveNext() && not (p e.Current)
           do yield e.Current
        }
        
let skip n (s:#seq<'a>) : seq<'a> =
    let e = s.GetEnumerator()
    let mutable index = 0
    while e.MoveNext() && index < n
        do index <- index + 1
    seq {
        yield e.Current
        while e.MoveNext()
            do yield e.Current
        }
        
let sumCount n (s:seq<int>) =
    let sumsInfo = new System.Collections.Generic.Dictionary<int * int, int>()
    let rec sumNoHigher n max (s:seq<int>) : int =
        if sumsInfo.ContainsKey( (n, max) ) then sumsInfo.[(n, max)]
        else
            let result =
                s |>
                truncateIf (fun pPrime -> pPrime > min n max) |>
                Seq.map (fun pPrime -> sumUsingMax n pPrime s) |>
                Seq.fold (+) 0
            sumsInfo.Add((n, max), result)
            result
    and sumUsingMax n max s : int =
        match max with
        | 0 when n <> 0 -> 0
        | 0 -> 1
        | _ when n < max -> 0
        | _ when n = max -> 1
        | 1 -> 1
        | _ -> sumNoHigher (n - max) max s
    sumNoHigher n n s
    
let print (s:#seq<'a>) : seq<'a> = Seq.map (fun (t:'a) -> printfn "%s" (t.ToString()) ; t) s

let nonNegs = Seq.initInfinite (fun i -> i)
let fibos = Seq.unfold (fun (x, y) -> Some (x, (y, x + y))) (1I, 1I)

// Both the next two fns should produce/identify primes up to 62,710,561
let isPrime (n:int) =
    if n = 2 || n = 3 then true
    elif n < 2 then false
    else
        let maxTest = int32(sqrt(float(n)))
        let lastTest = (primeList |> Seq.find (fun pTest -> pTest > maxTest || n % pTest = 0))
        lastTest > maxTest
    
let primes =
    Seq.append
        primeList
        (
            Seq.initInfinite (fun i -> 2 * i + 7921) |>    // Last prime in prime list is 7919
            Seq.filter (fun n -> isPrime n)
        )

// CAUTION: This only works for values that factor into Int32 primes (although it will
// will return [n] for bigints that can't be reduced further.  Thus it will work for
// primes and since composite values will have at least one factor less than sqrt(n)
// it will work for bigints less than (62 million)^2 so still probably okay.
let factorBig n = 
    let eStart = primes.GetEnumerator()
    let fAlwaysTrue = eStart.MoveNext()
    let rec factorHelper (n:bigint) (e:System.Collections.Generic.IEnumerator<int>) (l:List<bigint>) =
        let p = e.Current
        let bigP:bigint = bigint p
        match n with
        | x when x = 1I -> l
        // Relying on tail recursion in the following two recursions...
        | _ when n % bigP = 0I -> factorHelper (n / bigP) e (bigP :: l)
        | _ when e.MoveNext() -> factorHelper n e l
        | _ -> [n]
    factorHelper n eStart []

// for Int32 values a smaller version...
let factor n = 
    let eStart = primes.GetEnumerator()
    let fAlwaysTrue = eStart.MoveNext()
    let rec factorHelper n (e:System.Collections.Generic.IEnumerator<int>) (l:List<int>) =
        let p = e.Current
        match n with
        | 1 -> l
        // Relying on tail recursion in the following two recursions...
        | _ when n % p = 0 -> factorHelper (n / p) e (p :: l)
        | _ when e.MoveNext() -> factorHelper n e l
        | _ -> [n]  // Should never get here
    factorHelper n eStart []

let rec removeHeadDups l =
    match l with
    | [] | [_] -> []
    | _ ->
        let t = List.tail l
        if (List.head t) = List.head l then
            removeHeadDups t
        else
            t
    
let allFactors n =
    if n <= 0 then
        []
    else
        let factorSubsetSeq n = 
            let rec getsubsets (l) =
                match (l) with
                | [] -> []
                | _ -> 
                    let h = List.head l
                    let t = List.tail l
                    let d = removeHeadDups l
                    [[h]] |>
                    List.append (List.map (fun lt -> List.append [h] lt) (getsubsets t)) |>
                    List.append (getsubsets d)
            getsubsets (factor n)
        List.append [1] (List.map (fun l -> (Seq.fold ( * ) 1 l)) (factorSubsetSeq n))
    
let bigIntToNumericString (n:bigint) = n.ToString()
    
let rotate (s:string) (n:int) (cch:int) =
    s.Substring(n,(cch - n)) + s.Substring(0, n)
    
let reverse (s:string) =
    let n = String.length s
    let cs = Array.zeroCreate n
    for i = 1 to n do
        cs.[i - 1] <- s.[n - i]
    done
    new string(cs)

let reverseInt (n:int) =
    Int32.Parse(reverse (n.ToString()))
    
let reverseBigInt (n:bigint) =
    bigint.Parse (reverse (bigIntToNumericString n))
    
let rec GCD m n =
    if m < n then GCD n m
    elif m % n = 0 then n
    else GCD n (m % n)

let LCM m n =
    m * n / (GCD m n)

let rec bigGCD (m:bigint) (n:bigint) =
    if m < n then bigGCD n m
    elif m % n = 0I then n
    else bigGCD n (m % n)

let bigLCM (m:bigint) (n:bigint) =
    m * n / (bigGCD m n)

let rec bigFact (i:bigint) =
    match i with
    | _ when i <= 1I -> 1I
    | _ -> i * bigFact (i - 1I)

let rec fact i = bigFact (bigint (i:int32))

let isOdd n = (n &&& 1) <> 0
let isEven n = not (isOdd n)

let bigDigitSum (n:bigint) =
    bigIntToNumericString n |>
    Seq.toArray |>
    Array.map (fun c -> (int c) - (int '0')) |>
    Seq.fold (+) 0
    
let binomial m n = (fact m) / ( (fact n) * (fact (m - n)) )

let rec pow m n = if n = 0 then 1 else m * (pow m (n - 1))

let rec nestUntil f fTest n =
    let nNew = f n
    if (fTest n nNew) then nNew else (nestUntil f fTest nNew)

let iSqrt n =
    nestUntil (fun x -> (x + n / x ) / 2I) (fun n nNew -> n = nNew || n = nNew + 1I) (n/2I)

let isSquare(n:bigint) =
    let sqrt = iSqrt n
    sqrt * sqrt = n