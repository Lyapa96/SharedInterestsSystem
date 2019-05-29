import {randomInteger} from "./mathHelper";

export function createRandomPassengers(rows, columns, transportTypes) {
    let passengers = [];
    let currentPassengerNumber = 1;
    for (let i = 0; i < rows; i++) {
        let currentRow = [];
        for (let j = 0; j < columns; j++) {
            currentRow.push({
                id: currentPassengerNumber++,
                satisfaction: Number.parseFloat(Math.random().toFixed(2)),
                quality: Number.parseFloat(Math.random().toFixed(2)),
                transportType: transportTypes[randomInteger(0, 1)],
                coordinates: {i,j},
                allQualityCoefficients: []
            });
        }
        passengers.push(currentRow)
    }

    for (let i = 0; i < rows; i++) {
        for (let j = 0; j < columns; j++) {
            passengers[i][j].neighbors = getNeighbors(i,j,rows, columns, passengers);
        }
    }
    
    return passengers;
}

export function createRandomPassengers2(rows, columns, transportTypes) {
    let passengers = [];
    let count = rows * columns;
    for (let i = 0; i < count; i++){
        let passenger = {
            id: i + 1,
            satisfaction: Number.parseFloat(Math.random().toFixed(2)),
            quality: Number.parseFloat(Math.random().toFixed(2)),
            type: transportTypes[randomInteger(0, 1)],
            allQualityCoefficients: []
        };
        passengers.push(passenger);
    }
    //
    // for (let i = 0; i < rows; i++) {
    //     for (let j = 0; j < columns; j++) {
    //         passengers[i][j].neighbors = getNeighbors(i,j,rows, columns, passengers);
    //     }
    // }

    return passengers;
}

function getNeighbors(i, j, rows, columns, passengers) {
    let neighbors = [];
    for(let x = -1; x <= 1; x++) {
        for (let y = -1; y <= 1; y++) {
            if (Math.abs(x) + Math.abs(y) > 1){
                continue;
            }
            if (x === 0 && y === 0) {
                continue;
            }
            if (x + i >= rows || x + i < 0 || y + j >= columns || y + j < 0) {
                continue;
            }

            let neighbor = passengers[i + x][j + y];
            if (neighbor) {
                neighbors.push(neighbor.number);
            }
        }
    }
 
    return neighbors;
}