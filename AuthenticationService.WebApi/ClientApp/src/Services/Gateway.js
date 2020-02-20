
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
    }).
    then(async resp => {
        if(resp.ok){
            return await resp.json();
        } else {
            throw resp;
        }
    }).catch(err => {
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
                return null;
            default:
                return null;
        }
    })

const post = (path, body) =>
        fetch(`${safeBaseApiUrl}${path}`,{
            method: 'POST',
            credentials: 'include',
            headers:{
                'Content-Type': 'application/json',
            },
            body: body,
        })
        .then(response =>
            response
                .text()
                .then(text =>{
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
        )

export const Gateway = {
    get,
    post
}