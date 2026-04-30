[**lineup-client**](../../../index.md)

---

[lineup-client](../../../modules.md) / [utils/db](../index.md) / addToasts

# Function: addToasts()

> **addToasts**(`promise`, `loadingMessage?`, `successMessage?`): `void`

Defined in: [utils/db.tsx:12](https://github.com/rhyderswen/LineUp/blob/e2f17bae616784da1758a5d11711773573512737/lineup-client/src/utils/db.tsx#L12)

Adds toasts to asynchronous functions to give user's updates on their API queries.

## Parameters

### promise

`Promise`\<`any`\>

The Promise that updates the toast once resolved.

### loadingMessage?

`string`

The message to display while the Promise is still pending. Defaults to `"Submitting..."`

### successMessage?

`string`

The message to display if the Promise completes without error. Defaults to `"Success!"`

## Returns

`void`

## Example

```ts
addToasts(mutationVar.mutateAsync(), "Working on it...", "Done!");
```
