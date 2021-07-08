(define (domain aaqc)

    (:requirements
        :typing
        :strips
        :negative-preconditions
        :disjunctive-preconditions
        :conditional-effects
    )
    (:types 
        device sensor - object
        ventilation heater air-conditioner air-purifier - device
        temperature humidity air-purity co2-level - sensor
        temperature-in temperature-out - temperature
        humidity-out - humidity
        air-purity-in air-purity-out - air-purity
        co2-level-in - co2-level
    )

    (:predicates
        (on ?d - device)
        (temperature-high ?t - temperature)
        (temperature-low ?t - temperature)
        (humidity-high ?h - humidity)
        (humidity-low ?h - humidity)
        (air-purity-bad ?a - air-purity)
        (co2-level-emergency ?c - co2-level)
    )

    (:action activateVentilation
        :parameters (
            ?v - ventilation
            ?h - heater
            ?ac - air-conditioner
            ?ap - air-purifier
            ?ti - temperature-in
            ?to - temperature-out
            ?ho - humidity-out
            ?ai - air-purity-in
            ?ci - co2-level-in
        )
        :precondition (or 
            (and
                (and
                    (not(on ?h))
                    (not(on ?ac))
                    (not(on ?v))
                )
                (and
                    (or
                        (and
                            (temperature-high ?ti)
                            (temperature-low ?to)
                        )
                        (and
                            (temperature-low ?ti)
                            (temperature-high ?to)
                        )
                    )
                    (humidity-low ?ho)
                )
            )
            (co2-level-emergency ?ci)
        )
        :effect (and
            (on ?v)
            (not (temperature-low ?ti))
            (not (temperature-high ?ti))
            (not (air-purity-bad ?ai))
            (not (co2-level-emergency ?ci))
            (when
                (and (co2-level-emergency ?ci))
                (and 
                    (not(on ?h))
                    (not(on ?ac))
                    (not(on ?ap))
                )
            )
        )
    )

    (:action deactivateVentilation
        :parameters (
            ?v - ventilation
            ?h - heater
            ?ac - air-conditioner
            ?ti - temperature-in
            ?to - temperature-out
            ?ho - humidity-out
        )
        :precondition (and
            (and
                (not(on ?h))
                (not(on ?ac))
                (on ?v)
            )
            (or 
                (and
                    (not(temperature-low ?ti))
                    (not(temperature-high ?ti))
                )
                (temperature-high ?to)
                (temperature-low ?to)
                (humidity-high ?ho)
            )
        )
        :effect (
            not(on ?v)
        )
    )

    (:action activateHeater
        :parameters (
            ?v - ventilation
            ?h - heater
            ?ac - air-conditioner
            ?ti - temperature-in
            ?to - temperature-out
            ?ho - humidity-out
        )
        :precondition (and
            (and 
                (not(on ?h))
                (not(on ?ac))
                (not(on ?v))
            )
            (and
                (temperature-low ?ti)
                (or
                    (temperature-low ?to)
                    (and
                        (temperature-high ?to)
                        (humidity-high ?ho)
                    )
                )
            )
        )
        :effect (and
            (on ?h)
            (not (temperature-low ?ti))
            (not (temperature-high ?ti))
        )
    )

    (:action deactivateHeater
        :parameters (
            ?v - ventilation
            ?h - heater
            ?ac - air-conditioner
            ?ti - temperature-in
            ?to - temperature-out
            ?ho - humidity-out
        )
        :precondition (and
            (and
                (on ?h)
                (not(on ?ac))
                (not(on ?v))
            )
            (and
                (not(temperature-low ?ti))
                (not(temperature-high ?ti))
            )
        )
        :effect (
            not(on ?h)
        )
    )

    (:action activateAirCondition
        :parameters (
            ?v - ventilation
            ?h - heater
            ?ac - air-conditioner
            ?ti - temperature-in
            ?to - temperature-out
            ?ho - humidity-out
        )
        :precondition (and
            (and 
                (not(on ?h))
                (not(on ?ac))
                (not(on ?v))
            )
            (and
                (temperature-high ?ti)
                (or
                    (temperature-high ?to)
                    (and
                        (temperature-low ?to)
                        (humidity-high ?ho)
                    )
                )
            )
        )
        :effect (and
            (on ?ac)
            (not (temperature-low ?ti))
            (not (temperature-high ?ti))
        )
    )

    (:action deactivateAirCondition
        :parameters (
            ?v - ventilation
            ?h - heater
            ?ac - air-conditioner
            ?ti - temperature-in
            ?to - temperature-out
            ?ho - humidity-out
        )
        :precondition (and
            (and
                (not(on ?h))
                (on ?ac)
                (not(on ?v))
            )
            (and
                (not(temperature-low ?ti))
                (not(temperature-high ?ti))
            )
        )
        :effect (
            not(on ?ac)
        )
    )

    (:action activateAirPurifier
        :parameters (
            ?ap - air-purifier
            ?ai - air-purity-in
            ?ao - air-purity-out
        )
        :precondition (and 
            (not(on ?ap))
            (air-purity-bad ?ai)
        )
        :effect (and 
            (on ?ap)
            (not (air-purity-bad ?ai))
        )
    )

    (:action deactivateAirPurifier
        :parameters (
            ?ap - air-purifier
            ?ai - air-purity-in
            ?ao - air-purity-out
        )
        :precondition (and
            (on ?ap)
            (not (air-purity-bad ?ai))
        )
        :effect (
            not(on ?ap)
        )
    )
)
;            ?v - ventilation
;            ?h - heater
;            ?ac - air-conditioner
;            ?ap - air-purifier
;            ?ti - temperature-in
;            ?to - temperature-out
;            ?ho - humidity-out
;            ?ai - air-purity-in
;            ?ao - air-purity-out
;            ?ci - co2-level-in
