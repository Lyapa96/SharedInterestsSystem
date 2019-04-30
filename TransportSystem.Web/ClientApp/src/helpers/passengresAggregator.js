export function getDataToChart(passengers, stairs = false) {
    let aggregatedData = passengers.reduce((accumulator, passenger) => {
        accumulator[passenger.quality] = (accumulator[passenger.quality] || []).concat(passenger.agentId);
        return accumulator;
    }, {});

    let arrayAggregatedData = Object.keys(aggregatedData).reduce((arr, key) => {
        arr.push({quality: key, data: aggregatedData[key]});
        return arr;
    }, []).sort((x,y) => x.quality - y.quality);
    let values = arrayAggregatedData.map(x => x.data.length);
    if (stairs){
        values.sort((x,y) => x - y);
    }

    return {
        keys: arrayAggregatedData.map(x => x.quality),
        values: values,
        firstLine: Math.floor(arrayAggregatedData.length/3) + 0.5,
        secondLine: Math.floor(arrayAggregatedData.length * 2/3) + 0.5,
        minQualityValue: 0 - 1,
        maxQualityValue: arrayAggregatedData.length + 1
    }
}