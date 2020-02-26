import { AuthenticationService } from './AuthenticationService';

const apiVersion = "v1";
const baseApiUri = new URL(`${process.env.PUBLIC_URL}/api/${apiVersion}`, window.location);
const safeBaseApiUrl = baseApiUri.origin === window.location.origin ? baseApiUri.href : null;


const get = (path) =>
    fetch(`${safeBaseApiUrl}${path}`,{
        method: 'GET',
        credentials: 'include',
        headers:{
            'Content-Type': 'application/json',
        }
    })
    .then(async resp => {
        if(resp.ok){
            try{
            return await resp.clone().json();
            }catch(err){
                let test = await resp.clone().text();
                debugger;
            }
        } else {
            throw resp;
        }
    }).catch(err => {
        debugger;
        console.log(err);
        switch(err.status){
            case 301:
            case 302:
            case 303:
            case 304:
                console.log('Redirect to new location!');
                return null;
            case 400:
                console.log(err.statusText);
                return null;
            case 401:
            case 403: 
                console.log(err.statusText);
                AuthenticationService.signOut();
                return null;
            default:
                return null;
        }
    })

const post = (path, body, csrfToken) =>
    fetch(`${safeBaseApiUrl}${path}`,{
        method: 'POST',
        credentials: 'include',
        headers: cleanupHeadersForBlanks({
            'Content-Type': 'application/json',
            'RequestVerificationToken': csrfToken
        }),
        body: body,
    })
    .then(response => {
        if(response && response.status == 200){
            return response.text().then(text => {
                debugger;
                const data = text && JSON.parse(text)
                if(response.ok){
                    return data;
                }
                
                if([401, 403].indexOf(response.status) !== -1){
                    // logout
                }

                const error = (data && data.message) || response.statusText;
                return error;
            })
        }
        
        debugger;
    })
    .catch(err => {
        debugger;
    });

// remove any null/undefined keys from headers; as there's not point delivering such properties
const cleanupHeadersForBlanks = (headerObject) =>{
    Object
        .keys(headerObject)
        .forEach((key) => (
            headerObject[key] === null
         || headerObject[key] === undefined)
        && delete headerObject[key]);
    return headerObject;
}

export const Gateway = {
    get,
    post
}