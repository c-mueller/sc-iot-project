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
            ?ao - air-purity-out
            ?ci - co2-level-in
        )
        :precondition (or 
            (and
                (and
                    (not(on ?h))
                    (not(on ?ac))
                )
                (and
                    (or
                        (and
                            (temperature-high ?ti)
                            (or
                                (temperature-low ?to)
                                (not(temperature-high ?to))
                            )
                        )
                        (and
                            (temperature-low ?ti)
                            (or
                                (temperature-high ?to)
                                (not(temperature-low ?to))
                            )
                        )
                    )
                    (not(humidity-high ?ho))
                    (not(air-purity-bad ?ao))
                )
            )
            (co2-level-emergency ?ci)
        )
        :effect (and
            (on ?v)
            (not(temperature-low ?ti))
            (not(temperature-high ?ti))
            (not(air-purity-bad ?ai))
            (not(co2-level-emergency ?ci))
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
            ?ao - air-purity-out
            ?ci - co2-level-in
        )
        :precondition (and
            (on ?v)
            (or 
                (and
                    (not(temperature-low ?ti))
                    (not(temperature-high ?ti))
                )
                (temperature-high ?to)
                (temperature-low ?to)
                (humidity-high ?ho)
                (air-purity-bad ?ao)
            )
            (not(co2-level-emergency ?ci))
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
            ?ao - air-purity-out
            ?ci - co2-level-in
        )
        :precondition (and
            (and 
                (not(on ?ac))
                (not(on ?v))
            )
            (and
                (temperature-low ?ti)
                (or
                    (temperature-low ?to)
                    (and
                        (or
                            (temperature-high ?to)
                            (not(temperature-low ?to))
                        )
                        (or
                            (air-purity-bad ?ao)
                            (humidity-high ?ho)
                        )
                    )
                )
            )
            (not(co2-level-emergency ?ci))
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
            ?ci - co2-level-in
        )
        :precondition (and
            (and
                (not(on ?ac))
                (not(on ?v))
            )
            (or
                (or
                    (not(temperature-low ?ti))
                    (temperature-high ?ti)
                )
                (and
                    (temperature-low ?ti)
                    (on ?h)
                )
            )
            (not(co2-level-emergency ?ci))
        )
        :effect (
            not(on ?h)
        )
    )

    (:action activateAirConditioner
        :parameters (
            ?v - ventilation
            ?h - heater
            ?ac - air-conditioner
            ?ti - temperature-in
            ?to - temperature-out
            ?ho - humidity-out
            ?ao - air-purity-out
            ?ci - co2-level-in
        )
        :precondition (and
            (and 
                (not(on ?h))
                (not(on ?v))
            )
            (and
                (temperature-high ?ti)
                (or
                    (temperature-high ?to)
                    (and
                        (or
                            (temperature-low ?to)
                            (not(temperature-high ?to))
                        )
                        (or
                            (air-purity-bad ?ao)
                            (humidity-high ?ho)
                        )
                    )
                )
            )
            (not(co2-level-emergency ?ci))
        )
        :effect (and
            (on ?ac)
            (not (temperature-low ?ti))
            (not (temperature-high ?ti))
        )
    )

    (:action deactivateAirConditioner
        :parameters (
            ?v - ventilation
            ?h - heater
            ?ac - air-conditioner
            ?ti - temperature-in
            ?to - temperature-out
            ?ho - humidity-out
            ?ci - co2-level-in
        )
        :precondition (and
            (and
                (not(on ?h))
                (not(on ?v))
            )
            (or
                (or
                    (not(temperature-high ?ti))
                    (temperature-low ?ti)
                )
                (and
                    (temperature-high ?ti)
                    (on ?ac)
                )
            )
            (not(co2-level-emergency ?ci))
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
            ?ci - co2-level-in
        )
        :precondition (and
            (air-purity-bad ?ai)
            (not(co2-level-emergency ?ci))
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
            ?ci - co2-level-in
        )
        :precondition (and
            (not(air-purity-bad ?ai))
            (not(co2-level-emergency ?ci))
        )
        :effect (
            not(on ?ap)
        )
    )
)
