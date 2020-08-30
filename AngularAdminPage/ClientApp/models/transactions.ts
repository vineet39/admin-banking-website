export interface Transaction {
    transactionID: number;
    transactionType: string;
    accountNumber: number;
    destinationAccountNumber?: number;
    amount: number;
    comment?: string;
    modifyDate: string;
}
