import {GetTokenSilentlyOptions} from '@auth0/auth0-react';

type AuthorizeServiceProps = {
    getAccessTokenSilently: (options? : GetTokenSilentlyOptions | undefined) => Promise<string>;
}

export class AuthorizeService {
    token: string | undefined;

    getAccessTokenSilently: (options? : GetTokenSilentlyOptions | undefined) => Promise<string>;

    constructor(props: AuthorizeServiceProps) {
        this.getAccessTokenSilently = props.getAccessTokenSilently;
    }

    async getAccessToken() {
        return await this.getAccessTokenSilently();
    }

    async getAuthorizationHeader() {
        return { Authorization: `Bearer ${await this.getAccessToken()}` };
    }

    async getAuthorizationHeaderValue() {
        return (await this.getAuthorizationHeader()).Authorization;
    }
}