export const MainNav = [
    {
        icon: 'pe-7s-graph2',
        label: 'Activity',
        to: '#/dashboards/activity',
    },
];
export const AuthenticationSetupNav = [
    {
        icon: 'pe-7s-portfolio',
        label: 'Customers',
        content: [
            {
                label: 'List customers',
                to: '#/customers/list',
            },
            {
                label: 'Create new customer',
                to: '#/customers/add',

            },
        ],
    },
    {
        icon: 'pe-7s-browser',
        label: 'Auth clients',
        content: [
            {
                label: 'List clients',
                to: '#/clients/list',
            },
            {
                label: 'Create new client',
                to: '#/clients/add',
            },
        ],
    },
    {
        icon: 'pe-7s-server',
        label: 'Services/Resources',
        content: [
            {
                label: 'List services/resources',
                to: '#/resources/list',
            },
            {
                label: 'Introduce new service/resource',
                to: '#/resources/add',
            },
        ],
    },
    {
        icon: 'pe-7s-users',
        label: 'Users',
        content: [
            {
                label: 'List users',
                to: '#/users/list',
            },
            {
                label: 'Create new user',
                to: '#/users/add',
            },
        ],
    },
];

export const SystemNav = [
    {
        icon: 'pe-7s-config',
        label: 'Environment',
        to: '#/system/settings',
    },
];