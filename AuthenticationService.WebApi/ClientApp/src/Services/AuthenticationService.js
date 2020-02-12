import React from 'react';
import { BehaviorSubject } from 'rxjs';

const sessionObject = new BehaviorSubject(JSON.parse(localStorage.getItem('session')));

const getSession = () => {
    return sessionObject.value;
};

const signIn = ({ username, password }) => {
    let result = false;
    if (username === 'demo' && password == 'Q1w2e3r4!') {
        result = true;

        localStorage.setItem('session', JSON.stringify({
            username: username,
            expires: Date.now() + 60000  // expires in 60 sec
        }))

    }

    return result;
};

const signOut = () => {
    return true;
}

const isExpired = () => {
    let session = getSession();
    let result = session && session.expires > Date.now();

    if(!result)
    {
        localStorage.removeItem('session');
    }

    return !result
}

const isAuthenticated = () => {
    return !isExpired();
}

export const AuthenticationService = {
    session: sessionObject.asObservable(),
    getSession,
    signIn,
    signOut,
    isExpired,
    isAuthenticated
}