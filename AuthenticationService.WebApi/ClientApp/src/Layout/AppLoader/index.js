import React from 'react';

// const AppLoader = ({headerText}) => {
//     return (
//         <div className="loader-container bg-strong-bliss" >
//             <div className="loader-container-inner">
//                 <div className="spinner"></div>
//                 <h6 className="mt-3">
//                     {{headerText}}
//                 </h6>
//             </div>
//         </div>
//     );
// }

class AppLoader extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            headerText: this.props.headerText || 'Please wait while we load...',
            smallText: this.props.smallText
        }
    }

    render(){
        return(
            <div className="loader-container bg-strong-bliss" >
                <div className="loader-container-inner">
                    <div className="spinner"></div>
                    <h6 className="mt-3">
                        {this.state.headerText}
                        {this.state.smallText && <small>{this.state.smallText}</small>}
                    </h6>
                </div>
            </div>
        );
    }
}

export default AppLoader;