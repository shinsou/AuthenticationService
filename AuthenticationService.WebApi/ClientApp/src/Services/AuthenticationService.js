import React from 'react';
import { BehaviorSubject } from 'rxjs';
import { Gateway } from './Gateway';

const getSessionLocalStorage = () => {
    var session = localStorage.getItem('session');
    if(session == null || session.length < 1){
        localStorage.removeItem('session');
        return null;
    }

    return session;
}

const sessionObject$ = new BehaviorSubject(JSON.parse(getSessionLocalStorage()));

const getLoginModel = () =>
    Gateway.get('/login');

const getSession = async () =>
    await Gateway
        .get('/session')
        .then(resp => {
            sessionObject$.subscribe(resp);
            return resp;
        });

const signIn = async ({ username, password, csrfToken }) => 
    await Gateway.post('/login', JSON.stringify({username, password}), csrfToken)
        .then(user => {
            localStorage.setItem('session', user);
            sessionObject$.next(user);

            return user;
        })

const signOut = () => {
    return true;
}

// const isExpired = async () => {
//     let session = await getSession();
//     let result = session && session.expires > Date.now();

//     if(!result) {
//         localStorage.removeItem('session');
//     }
// debugger;
//     return !result
// }

// const isAuthenticated = async () =>
//     await isExpired()
    

export const AuthenticationService = {
    session: sessionObject$.asObservable(),
    get sessionValue () { return sessionObject$.value; },
    getSession,
    getLoginModel,
    signIn,
    signOut,
    //isExpired,
    //isAuthenticated
}