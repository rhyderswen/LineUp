[**lineup-client**](../../../../index.md)

---

[lineup-client](../../../../modules.md) / [utils/api/auth-token](../index.md) / logout

# Function: logout()

> **logout**(`options?`): `Promise`\<`void`\>

Defined in: [utils/api/auth-token.ts:46](https://github.com/rhyderswen/LineUp/blob/e2f17bae616784da1758a5d11711773573512737/lineup-client/src/utils/api/auth-token.ts#L46)

Logs the user out once [registerLogout](registerLogout.md) has been successfully called.

## Parameters

### options?

#### logoutParams?

\{ `returnTo?`: `string`; \}

#### logoutParams.returnTo?

`string`

## Returns

`Promise`\<`void`\>

## Remarks

To be used ONLY in a place where hooks are not available (i.e. in loaders).
