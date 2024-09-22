import DrawerNav from "../DrawerNav"
import ChatBox from "./ChatBox"
import "./chat.css"

function Chat() {
    return (
        <DrawerNav>
            <div className="chatContainer">
                <ChatBox />
            </div>
        </DrawerNav>
    )
}

export default Chat