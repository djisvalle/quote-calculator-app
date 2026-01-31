import { Button } from "@/components/ui/button";
import { useNavigate } from "react-router-dom";

export default function Home() {
    const navigate = useNavigate();

    return (
        <div className="min-h-screen flex flex-col items-center justify-between p-12 bg-slate-50 text-slate-900">
            <div aria-hidden="true" />

            <main className="text-center space-y-6">
                <h1 className="text-9xl font-black tracking-tighter uppercase">
                    Quote Calculator
                </h1>
                
                <p className="text-xl font-medium tracking-wide text-cyan-600 uppercase tracking-widest">
                    MONEYME Coding Challenge
                </p>
            </main>

            <footer className="text-sm font-semibold text-slate-400 uppercase tracking-widest">
                January 2026
            </footer>
        </div>
    );
}