import React from 'react';
//import AuthService from './Services/AuthService';
import { Redirect, Route } from 'react-router-dom';
import { AuthenticationService } from './Services/AuthenticationService';

const PrivateRoute = ({component: Component, ...rest}) => {

    const isLoggedIn = AuthenticationService.isAuthenticated();

    return (
        <Route
            {...rest}
            render={props =>
                isLoggedIn
                    ? <Component {...props} />
                    : <Redirect to={{ pathname: '/login', state: { from:  props.location} }} />
            }
        />
    );
};

export default PrivateRoute;