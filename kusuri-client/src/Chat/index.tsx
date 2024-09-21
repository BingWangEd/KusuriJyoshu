import DrawerNav from "../DrawerNav"
import ChatBox from "./ChatBox"
import ChatHistory from "./ChatHistory"
import "./chat.css"

function Chat() {
    return (
        <DrawerNav>
            <div className="chatContainer">
                <div className="chatHistory">
                    <div className="buffer"></div>
                    <ChatHistory />
                </div>
                <ChatBox />
            </div>
        </DrawerNav>
    )
}

export default Chat