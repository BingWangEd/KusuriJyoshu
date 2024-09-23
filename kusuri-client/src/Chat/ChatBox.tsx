import { TextField } from "@mui/material";
import { useCallback, useEffect, useRef, useState } from "react";
import ArrowCircleUpIcon from '@mui/icons-material/ArrowCircleUp';
import ChatHistory from "./ChatHistory";
import LoadingButton from '@mui/lab/LoadingButton';
import SaveIcon from '@mui/icons-material/Save';

interface IChatBox {

}

export interface IChat{
    id: number | null,
    content: string,
    patientId: number,
    byBot: boolean,
    createdAt: string | null,
}

const ChatBox = ({  }: IChatBox) => {
    const patientId = 2;
    const [message, setMessage] = useState("");
    const [history, setHistory] = useState<IChat[]>([]);
    const [sending, setSending] = useState(false);

    const dummyLastRef = useRef<HTMLDivElement>(null);
    const previousHistory = useRef<IChat[] | undefined>();

    const getHistory = useCallback(async () => {
        try {
            const response = await fetch(`/api/Chat/GetHistory/${patientId}`, {
              method: "GET",
              headers: { "Content-Type": "application/json" }
            });
      
            if (!response.ok) {
              throw new Error("Network response was not ok");
            }

            const result = await response.json();
            setHistory(result);
        } catch (error) {
            console.error(error);
        }
    }, []);

    const sendMessage = useCallback(async () => {
        setSending(true);
        try {
            const response = await fetch(`/api/Chat/${patientId}`, {
              method: "POST",
              headers: { "Content-Type": "application/json" },
              body: JSON.stringify(message)
            });
      
            if (!response.ok) {
              throw new Error("Network response was not ok");
            }

            setMessage("");
            console.log(response);
            
            await getHistory();
        } catch (error) {
            console.error(error);
        }
        setSending(false);
    }, [message]);

    useEffect(() => {
        if (!previousHistory.current && history.length > 0) {
            dummyLastRef.current?.scrollIntoView();
        }
    
        if (history.length > 0) {
            previousHistory.current = history;
        }
    }, [history])

    useEffect(() => {
        getHistory();
	}, []);

    useEffect(() => {
        dummyLastRef.current?.scrollIntoView();
    }, [history.length]);

	return (
        <>
            <div className="chatHistory">
                <div className="buffer"></div>
                <ChatHistory history={history} />
                <div ref={dummyLastRef}></div>
            </div>
            <div className="chatBox">
                <TextField
                    id="outlined-multiline-static"
                    label="メッセージを入力してください"
                    multiline
                    rows={1}
                    value={message}
                    onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
                        setMessage(event.target.value);
                    }}
                    style={{ flexGrow: 1 }}
                />
                {sending ? (
                    <LoadingButton loading loadingPosition="start" startIcon={<SaveIcon />}>
                        送信中...
                    </LoadingButton>
                ) : (
                    <button onClick={sendMessage} style={{ marginLeft: '8px'}}><ArrowCircleUpIcon /></button>
                )}
            </div>
        </>
	);
}

export default ChatBox;
