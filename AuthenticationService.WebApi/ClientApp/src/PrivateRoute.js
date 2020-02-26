import React, { Component, lazy } from 'react';
//import AuthService from './Services/AuthService';
import { Redirect, Route } from 'react-router-dom';
import { AuthenticationService } from './Services/AuthenticationService';
import { ThemeConsumer } from 'styled-components';

class PrivateRoute extends Component {
    constructor(props) {
        super(props);
        this.state = {
            loaded: false,
            loading: false,
            isAuthenticated: false
        }
    }

    componentDidMount(){
        this.authenticate();
    }

    authenticate(){
        const { loading } = this.state;
        if(!loading){
            this.setState({loading: true});
            AuthenticationService.isAuthenticated()
            .then(response => {
                this.setState({loading: false, loaded: true, isAuthenticated: response});
            });
        }
    }

    render() {
        const { component: Component, ...rest } = this.props;
        const { loaded, isAuthenticated} = this.state
        
        if(!loaded){
            return null;
        }
        
        return (
            <Route
                {...rest}
                render={
                    props => {
                        return isAuthenticated 
                            ? (<Component {...props} />)
                            : (<Redirect to={{ pathname: '/login', state: { from:  this.props.location} }} />)
                    }
                }
            />
        )
    }
}

export default PrivateRoute;