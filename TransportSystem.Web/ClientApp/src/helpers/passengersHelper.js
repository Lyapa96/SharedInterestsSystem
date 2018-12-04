import {randomInteger} from "./mathHelper";

export function createRandomPassengers(rows, columns, transportTypes) {
    let passengers = [];
    for (let i = 0; i < rows; i++) {
        let currentRow = [];
        for (let j = 0; j < columns; j++) {
            currentRow.push({
                number: `${i} ${j}`,
                satisfaction: Math.random().toFixed(2),
                quality: Math.random().toFixed(2),
                transportType: transportTypes[randomInteger(0, 1)]
            });
        }
        passengers.push(currentRow)
    }

    return passengers;
}