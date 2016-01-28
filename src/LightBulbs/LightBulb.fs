namespace LightBulbs

open MathNet.Numerics
open MathNet.Numerics.Distributions

module Discounting =
       let YearlyToHourly (r: float<1/year>) :float<1/h> = r/HoursPerYear
       let YearlyToMonthly (r: float<1/year>) :float<1/month> = r/MonthsPerYear
       
       let Discount (rate: float<1/'t>) (amounts: seq<float<'t>*float<USD>>) =
         amounts
         |> Seq.map (fun (t, x) -> x*(exp (-rate*t)))
         |> Seq.sum
         


module LightBulb =     

    let Rate (bulb: LightBulb) = (log 2.0)/bulb.Lifetime
    
    let Sample (bulb: LightBulb) = 
        let lambda = float (Rate bulb)
        Exponential.Sample lambda * 1.0<h> 
    
    let Samples (bulb: LightBulb) = 
        let lambda = float (Rate bulb)
        Exponential.Samples lambda
        |> Seq.map (fun x -> 1.0<h>*x)

    let RunningCost (bulb: LightBulb) (powerCost: float<USD/(kWh)>) (time: float<h>) :float<USD>=
        time*(bulb.Watts/WattsPerKiloWatt)*powerCost

    let SimBulbsNeeded (bulb: LightBulb) (totalHours: float<h>) =
        Samples bulb
        |> Seq.scan (+) 0.0<h>
        |> Seq.takeWhile (fun x -> x < totalHours)
        //Without this List.ofSeq call behavior is not what is intended--new random numbers are pulled on access!
        |> List.ofSeq

    let ReplacementCost (cost: float<USD>) (inflation: float<1/'t>) (discountRate: float<1/'t>) (times: list<float<'t>>) =
        let costs = Seq.map (fun x -> cost * (exp (inflation*x))) times
        Discounting.Discount discountRate (Seq.zip times costs)


