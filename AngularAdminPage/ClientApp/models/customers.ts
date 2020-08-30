import { Login } from "./login";

export interface Customer {
    customerID: number;
    customerName: string;
    tfn?: string;
    address?: string;
    city?: string;
    state?: string;
    postCode?: string;
    phone: string;
    login: Login;
}
