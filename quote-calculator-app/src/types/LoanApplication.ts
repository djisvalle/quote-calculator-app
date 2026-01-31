export interface LoanApplication {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    amount: number;
    term: number;
}

export interface LoanApplicationDetails {
    loanApplicationPublicId: string;
    applicantId: number;
    title: string;
    firstName: string;
    lastName: string;
    email: string;
    mobileNumber: string;
    dateOfBirth: Date;
    amount: number;
    term: number;
    productType: number;
}

export interface CalculateLoanRequest {
    productTypeId: number | undefined;
    amount: number | undefined;
    term: number | undefined;
}

export interface DraftLoanApplicationRequest {
    loanApplicationPublicId: string;
    applicantId: number;
    title: string;
    firstName: string;
    lastName: string;
    email: string;
    mobileNumber: string;
    amount: number;
    term: number;
    productTypeId: number;
}

export interface CalculateLoanDetails {
    amount: number;
    term: number;
    establishmentFee: number;
    interestFee: number;
    weeklyRepayment: number;
    monthlyRepayment: number;
    totalRepayment: number;
}

export interface FinalizeLoanApplicationRequest {
    loanApplicationPublicId: string;
    applicantId: number;
    firstName: string;
    lastName: string;
    email: string;
    mobileNumber: string;
    amount: number;
    term: number;
    interestFee: number;
    weeklyRepayment: number;
    monthlyRepayment: number;
    totalRepayment: number;
}

export interface ApplicationReceiptDetails {
    firstName: string;
    email: string;
    amount: number;
    referenceNumber: string;
    status: number;
}