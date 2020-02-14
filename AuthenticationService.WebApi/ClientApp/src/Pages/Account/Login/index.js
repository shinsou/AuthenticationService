import React, {Fragment, Component} from "react";
import { AuthenticationService } from '../../../Services/AuthenticationService';
import Slider from "react-slick";
import FloatingLabel from "floating-label-react";
import "floating-label-react/styles.css";
import bg1 from '../../../assets/utils/images/originals/1673477.jpg';
import bg2 from '../../../assets/utils/images/originals/1673364.png';
import bg3 from '../../../assets/utils/images/originals/1673318.jpg';

import {Col, Row, Button, Form, FormGroup, Label, Input} from 'reactstrap';

export default class Login extends Component {
    constructor(props){
        super(props);
        this.state = {
            username: 'demo',
            password: 'Q1w2e3r4!'
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleChange(event) {
        this.setState({[event.target.name]: event.target.value});
    }
    
    handleSubmit(event) {
        event.preventDefault();
        let res = AuthenticationService.signIn({ username: this.state.username, password: this.state.password});
        
        if(res)
            window.location.pathname = "/";
        //else
        // show error
    }

    render() {
        let sliderSettings = {
            dots: true,
            infinite: true,
            speed: 1000,
            arrows: true,
            slidesToShow: 1,
            slidesToScroll: 1,
            fade: true,
            initialSlide: 0,
            autoplay: true,
            adaptiveHeight: true
        };
        return (

            <Fragment>
                <div className="h-100">
                    <Row className="h-100 no-gutters">
                        <Col lg="8" className="d-none d-lg-block">
                            <div className="slider-light">
                                <Slider  {...sliderSettings}>
                                    <div
                                        className="h-100 d-flex justify-content-center align-items-center bg-plum-plate">
                                        <div className="slide-img-bg"
                                             style={{
                                                 backgroundImage: 'url(' + bg3 + ')'
                                             }}/>
                                        <div className="slider-content">
                                            <h2>Trust, it's safe!</h2>
                                            <p>
                                                Uses only high quality and safe/certified code base
                                            </p>
                                        </div>
                                    </div>
                                    <div
                                        className="h-100 d-flex justify-content-center align-items-center bg-premium-dark">
                                        <div className="slide-img-bg"
                                             style={{
                                                 backgroundImage: 'url(' + bg1 + ')'
                                             }}/>
                                        <div className="slider-content">
                                            <h2>Scalable, Modular, Consistent</h2>
                                            <p>
                                                Coded with quality and far designed architecture! 
                                            </p>
                                        </div>
                                    </div>
                                    <div
                                        className="h-100 d-flex justify-content-center align-items-center bg-sunny-morning">
                                        <div className="slide-img-bg opacity-6"
                                             style={{
                                                 backgroundImage: 'url(' + bg2 + ')'
                                             }}/>
                                        <div className="slider-content">
                                            <h2>Try it!</h2>
                                            <p>
                                                and while you at it, send some feedback!
                                            </p>
                                        </div>
                                    </div>
                                </Slider>
                            </div>
                        </Col>
                        <Col lg="4" md="12" className="h-100 d-flex bg-white justify-content-center align-items-center">
                            <Col sm="11" className="mx-auto app-login-box">
                                <div className="app-logo"/>
                                <Row className="divider"/>
                                <div>
                                    <Form onSubmit={this.handleSubmit}>
                                        <Row form>
                                            <Col md={12}>
                                                    <FloatingLabel
                                                        id="username"
                                                        name="username"
                                                        placeholder="Username"
                                                        type="text"
                                                        value={this.state.username}
                                                        onChange={this.handleChange}
                                                        />
                                            </Col>
                                            <Col md={12}>
                                                <FormGroup>
                                                    <FloatingLabel
                                                        id="password"
                                                        name="password"
                                                        placeholder="Password"
                                                        type="password"
                                                        value={this.state.password}
                                                        onChange={this.handleChange}
                                                        />
                                                </FormGroup>
                                            </Col>
                                        </Row>
                                        <FormGroup check className="">
                                            <Input type="checkbox" name="check" id="exampleCheck" className="" />
                                            <Label for="exampleCheck" check>Keep me logged in</Label>
                                        </FormGroup>
                                        <Row className="divider"/>
                                        <div className="d-flex align-items-center">
                                            <div className="ml-auto">
                                                <Button color="primary" size="lg">Login to Dashboard</Button>
                                            </div>
                                        </div>
                                    </Form>
                                </div>
                            </Col>
                        </Col>
                    </Row>
                </div>
            </Fragment>
        );
    }
}
