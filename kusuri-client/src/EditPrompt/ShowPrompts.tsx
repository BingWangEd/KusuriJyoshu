import { usePrescriptionContext } from "./usePrescriptionContext";
import PromptCard from "./PromptCard";

function ShowPrompts() {
    const prompts = usePrescriptionContext();

    return (
        <>
            {
                prompts.length > 0 && prompts.map(
                    p => <PromptCard key={p.id} id={p.id} date={p.date} content={p.content} patientId={p.patientId}/>
                )
            }
        </>
    )
}

export default ShowPrompts