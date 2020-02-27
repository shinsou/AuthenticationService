import React from 'react';
import { Gateway } from './Gateway';

const getClients = () =>
    Gateway.get('/clients')
        .then(response => response);

export const ClientGateway = {
    getClients
}