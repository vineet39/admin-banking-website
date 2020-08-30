export interface Billpay {
    billPayID: number;
    accountNumber: Number;
    payeeID: number;
    amount: number;
    scheduleDate: string;
    period: string;
    modifyDate: string;
    locked: boolean;
}
