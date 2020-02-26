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
            if(resp){
                sessionObject$.subscribe(resp);
            }
            return resp;
        }).catch(err => {
            debugger;
        });

const signIn = async ({ username, password, rememberLogin, returnUrl, csrfToken }) => 
    await Gateway.post('/login', JSON.stringify({username, password, rememberLogin, returnUrl}), csrfToken)
        .then(loginResponse => loginResponse)
        .then(loginResponse => getSession()
            .then(sessionResponse => {
                if(sessionResponse){
                    localStorage.setItem('session', JSON.stringify(sessionResponse));
                    sessionObject$.next(sessionResponse);
                }

                return loginResponse;
            }))
        .catch(err => {
            return err;
        });

const signOut = () => {
    return true;
}

const isExpired = () =>
    getSession()
        .then(sessionResponse =>
            sessionResponse 
                ? false
                : true)
        .catch(err => {
            debugger;
        })

const isAuthenticated = () => {
    let hasActiveSession = sessionObject$.value != null;
    if(!hasActiveSession)
        return Promise.resolve(false);

    return isExpired()
    .then(expired => {
        return sessionObject$.value && !expired
            ? true
            : false;
    });
}

export const AuthenticationService = {
    session: sessionObject$.asObservable(),
    get sessionValue () { return sessionObject$.value; },
    getSession,
    getLoginModel,
    signIn,
    signOut,
    //isExpired,
    isAuthenticated
}