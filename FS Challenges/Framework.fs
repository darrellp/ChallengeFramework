module FS_Challenges
open System.IO
open System

type ChallengeAttribute(contest:string, name:string, uri:string) =
    inherit System.Attribute()
   
    member this.Contest = contest
    member this.Name = name
    member this.URI = new Uri(uri)

type IChallenge =
     abstract Solve : System.IO.StringReader -> string
     abstract RetrieveSampleInput : unit -> string
     abstract RetrieveSampleOutput : unit -> string
