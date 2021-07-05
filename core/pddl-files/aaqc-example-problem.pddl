(define (problem testProblem) (:domain aaqc)
;this is an example problem as should be constructed by our application
;in this example we will start in our applications initial state where all
;actuators are off and go into a state where the air purifier and heater are on
(:objects 
    v - ventilation
    h - heater
    ac - air-conditioner
    ap - air-purifier
    ti - temperature-in
    to - temperature-out
    ho - humidity-out
    ai - air-purity-in
    ao - air-purity-out
    ci - co2-level-in
)

;The inital variable states have to be constructed by the application outside of this example problem
(:init
    (temperature-low to)
    (temperature-low ti)
    (humidity-low ho)
    (air-purity-bad ai)
    (air-purity-bad ao)
)

;The goal has to be constructed by the application outside of this example problem
(:goal (and
    (on h)
    (on ap)
))

;un-comment the following line if metric is needed
;(:metric minimize (???))
)
