import {AxiosRequestConfig} from "axios";
import {AuthorizeService} from "./AuthorizeService.ts";

export class AuthorizedApiBase {

    private readonly authService: AuthorizeService;
    constructor(authService: AuthorizeService) {
        this.authService = authService;
    }

    async transformOptions(options: AxiosRequestConfig) : Promise<AxiosRequestConfig> {
        options.headers = {
            ...options.headers,
            Authorization: "Bearer " + await this.authService.getAccessToken(),
        };

        return Promise.resolve(options);
    }

    getBaseUrl(defaultUrl?: string) : string {
        return defaultUrl || import.meta.env.VITE_API_BASE_URL;
    }
}