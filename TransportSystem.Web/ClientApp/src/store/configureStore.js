import { applyMiddleware, combineReducers, compose, createStore } from 'redux';
import thunk from 'redux-thunk';
import { routerReducer, routerMiddleware } from 'react-router-redux';
import * as Passengers from './Passengers';
import * as Smo from './Smo.js';

export default function configureStore(history, initialState) {
    const reducers = {
        passengers: Passengers.reducer,
        smo: Smo.reducer
    };

    const logger = store => next => action => {
        let result;
        console.groupCollapsed("dispatching", action.type);
        console.log('prev state', store.getState());
        console.log('action', action);
        result = next(action);
        console.log('next state', store.getState());
        console.groupEnd();
        return result;
    };

    const middleware = [
        thunk,
        logger,
        routerMiddleware(history)
    ];

    // In development, use the browser's Redux dev tools extension if installed
    const enhancers = [];
    const isDevelopment = process.env.NODE_ENV === 'development';
    if (isDevelopment && typeof window !== 'undefined' && window.devToolsExtension) {
        enhancers.push(window.devToolsExtension());
    }

    const rootReducer = combineReducers({
        ...reducers,
        routing: routerReducer
    });

    return createStore(
        rootReducer,
        initialState,
        compose(applyMiddleware(...middleware), ...enhancers)
    );
}