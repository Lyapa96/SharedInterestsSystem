const setInitStateType = 'SET_INIT_STATE_FOR_TRANSPORTS';
const setMainProperties = 'SET_MAIN_PROPERTIES_TRANSPORT';
const runNextStepForTransports = 'RUN_NEXT_STEP_FOR_TRANSPORTS';
const runNextOneHundredStepsForTransports = 'RUN_NEXT_ONE_HUNDRED_STEPS_FOR_TRANSPORTS';

const initialState = {
    isInitState: true,
    isInteractiveMode: false,
    iterationResults: []
};
//const url = 'https://transportsysteminfra.azurewebsites.net/api/passengers/';
const url = `https://localhost:5003/api/passengers/`;

export const actionCreators = {
    setInitState: () => ({type: setInitStateType}),
    setMainPropertiesTransport: (data) => async (dispatch) => {
        let passengersCount = Number(data.passengersCount);
        let columns = Number(data.columns);
        let neighboursCount = Number(data.neighboursCount);
        let availableTransportTypes = data.availableTransportTypes
            .filter(x => x.active)
            .map(x => x.type);

        let body = JSON.stringify({
            columns: columns,
            passengersCount: passengersCount,
            neighboursCount: neighboursCount,
            availableTransportTypes: availableTransportTypes
        });
        const method = 'initWithTransport';
        const response = await fetch(url + method, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: "POST",
            body: body
        });
        const iterationResult = await response.json();
        dispatch({
            type: setMainProperties,
            payload: {
                iterationResult: iterationResult,
                columns: columns
            }
        });
    },
    runNextStepTransports: () => async (dispatch, getState) => {
        let transportsState = getState().transports;
        let currentStep = transportsState.iterationResults[transportsState.iterationResults.length - 1];
        let method = 'nextStep';
        const response = await fetch(url + method, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: "POST",
            body: JSON.stringify(currentStep)
        });
        const nextStep = await response.json();
        dispatch({type: runNextStepForTransports, payload: nextStep});
    },
    runNextOneHundredStepsTransports: () => async (dispatch, getState) => {
        let transportsState = getState().transports;
        let currentStep = transportsState.iterationResults[transportsState.iterationResults.length - 1];
        let method = 'nextSteps';
        const response = await fetch(url + method, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: "POST",
            body: JSON.stringify(currentStep)
        });
        const nextStep = await response.json();
        dispatch({type: runNextOneHundredStepsForTransports, payload: nextStep});
    }
};

export const reducer = (state, action) => {
    state = state || initialState;

    if (action.type === setInitStateType) {
        return initialState;
    }

    if (action.type === setMainProperties) {
        let newIterationResults = [
            ...state.iterationResults,
        ];
        newIterationResults.push(action.payload.iterationResult);
        return {
            isInitState: false,
            isInteractiveMode: true,
            iterationResults: newIterationResults
        }
    }

    if (action.type === runNextStepForTransports) {
        let newIterationResults = [
            ...state.iterationResults,
        ];
        newIterationResults.push(action.payload);
        return {
            ...state,
            iterationResults: newIterationResults
        }
    }

    if (action.type === runNextOneHundredStepsForTransports) {
        let newIterationResults = [
            ...state.iterationResults,
        ];
        newIterationResults = newIterationResults.concat(action.payload);
        return {
            ...state,
            iterationResults: newIterationResults
        }
    }

    return state;
};

