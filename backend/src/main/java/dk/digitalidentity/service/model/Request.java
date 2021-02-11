package dk.digitalidentity.service.model;

import java.util.Objects;
import java.util.UUID;

import dk.digitalidentity.service.HMacUtil;
import lombok.Getter;
import lombok.Setter;
import lombok.extern.slf4j.Slf4j;

@Slf4j
@Getter
@Setter
public class Request {
	private String transactionUuid;     // random uuid
	private String command;             // AUTHENTICATE | GET_USERS_WITH_ROLE
	private String payload;             // set to roleIdentifier for GET_USERS_WITH_ROLE
	private String signature;           // keyed hmac of above
	
	public Request() {
		this.transactionUuid = UUID.randomUUID().toString();
	}
	
	public void sign(String key) throws Exception {
		switch (command) {
			case "AUTHENTICATE":
				this.signature = HMacUtil.hmac(transactionUuid + "." + command, key);
				break;
			case "GET_USERS_WITH_ROLE":
				this.signature = HMacUtil.hmac(transactionUuid + "." + command + "." + payload, key);
				break;
			default:
				throw new Exception("Unknown command: " + command);
		}
	}

	public boolean validateEcho(Response message) {
		switch (command) {
			case "AUTHENTICATE":
				if (Objects.equals(command, message.getCommand())) {
					return true;
				}
				break;
			case "GET_USERS_WITH_ROLE":
				if (Objects.equals(command, message.getCommand())) {
					return true;
				}
				break;
			default:
				log.error("Unknown command: " + command);
		}

		return false;
	}
}
