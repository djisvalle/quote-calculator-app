import type {
    DraftLoanApplicationRequest,
    CalculateLoanRequest,
    CalculateLoanDetails,
    FinalizeLoanApplicationRequest,
    LoanApplicationDetails,
    ApplicationReceiptDetails
} from "@/types/LoanApplication.ts";

import { http } from "./http.ts";

export const loanApi = {
    getDetailsByPublicId: async (publicId: string | null) => {
        const res = await http.get<LoanApplicationDetails>(`/loan/${publicId}`);
        return res.data
    },

    saveDraftLoanApplication: async (draftLoanApplicationRequest: DraftLoanApplicationRequest) => {
        const res = await http.patch<LoanApplicationDetails>(`/loan/save-draft/${draftLoanApplicationRequest.loanApplicationPublicId}`, draftLoanApplicationRequest);
        return res.data;
    },

    calculateLoanApplication: async (calculateLoanDto: CalculateLoanRequest) => {
        const res = await http.post<CalculateLoanDetails>("/loan/calculate", calculateLoanDto);
        return res.data;
    },

    finalizeLoanApplication: async (finalizeLoanApplicationRequest: FinalizeLoanApplicationRequest) => {
        const res = await http.put<LoanApplicationDetails>(`/loan/${finalizeLoanApplicationRequest.loanApplicationPublicId}`, finalizeLoanApplicationRequest);
        return res.data;
    },

    getReceiptDetailsByPublicId: async (publicId: string | null) => {
        const res = await http.get<ApplicationReceiptDetails>(`/loan/receipt-details/${publicId}`);
        return res.data
    },
}